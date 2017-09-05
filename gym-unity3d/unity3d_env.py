import socket
import subprocess
import sys
import struct
import random
import numpy as np
import cv2
import binascii
import functools
import os, time
from operator import mul

SIZE_OF_UNITY_PIXEL_CHANNEL = 4
SCENE_OP_CODE_ACTION = 0x01ff
SCENE_OP_CODE_RESET = 0xff
SCENE_OP_CODE_NOOP = 0x00

def image_rgba32_to_bgr(data):
    data = (data[:,:,:3] * 255.0).astype(np.uint8)
    data = cv2.cvtColor(data, cv2.COLOR_RGB2BGR)
    return data
    
def image_unity3d_to_cv2(data):
    data = image_rgba32_to_bgr(data)
    data = np.flip(data, axis=0)
    return data
    
def recvall(sock, count):
    buf = b''
    while count:
        newbuf = sock.recv(count)
        if not newbuf: return None
        buf += newbuf
        count -= len(newbuf)
    return buf

class DiscreteActionSpace(object):
    def __init__(self, num_action):
        self.n = num_action
        
    def sample(self):
        return random.randint(0, self.n)
        
    def pack(self, act):
        return struct.pack("II", SCENE_OP_CODE_ACTION, act)
    
    def reset(self, act):
        return struct.pack("II", SCENE_OP_CODE_RESET, 0)
        
class ContinuousActionSpace(object):
    def __init__(self, num_dim):
        self.n = num_dim
        self.mu = 0.0
        self.sigma = 1.0
        self.pack_format = 'I' + ''.join('f' * self.n)
        
    def sample(self):
        return np.random.normal(self.mu, self.sigma, self.n)
        
    def pack(self, act):
        return struct.pack(self.pack_format, SCENE_OP_CODE_ACTION, *act)
        
    def reset(self):
        act = np.zeros(self.n, dtype=np.float32)
        return struct.pack(self.pack_format, SCENE_OP_CODE_RESET, *act)
    
    def noop(self):
        act = np.zeros(self.n, dtype=np.float32)
        return struct.pack(self.pack_format, SCENE_OP_CODE_NOOP, *act)
                
class ObservationSpace(object):
    def observe(self, raw_data):
        raise NotImplementedError()
        
class VisionObservationSpace(ObservationSpace):
    def __init__(self, shape):
        self.shape = shape
        self.size = functools.reduce(mul, shape) * SIZE_OF_UNITY_PIXEL_CHANNEL
        
    def observe(self, raw_data):
        observation = np.fromstring(raw_data, np.float32)
        observation = observation.reshape(self.shape)
        observation = image_unity3d_to_cv2(observation)
        return observation
    
class ScalarRewardSpace(object):
    def __init__(self):
        self.size = 4

    def unpack(self, raw_data):
        reward = np.fromstring(raw_data, np.float32)
        return reward[0]

class Unity3DEnvironment(object):
    def __init__(self, server_address=('127.0.0.1', 8888), scene='navigation-v0'):
        # Open Unity3D
        #cmd = 'DISPLAY=:0 $UNITY_SCENES/%s/%s.x86_64 port %d' % (scene, scene, server_address[1])
        #self.proc = subprocess.Popen([cmd], shell=True)
        #time.sleep(3)
        #pid = self.proc.pid

        # Create a TCP/IP socket
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        
        # Connect the socket to the port where the server is listening
        self.server_address = server_address
        self.sock.connect(server_address)
        
        # TODO: Scene creation schemea
        
        # TODO: Define action space according to scene
        self.action_space = ContinuousActionSpace(2)
        
        # TODO: Define observation space according to scene
        # Now testing default size=256*256*4 observation space
        self.observation_space = VisionObservationSpace((256, 256, 4))
        
        # TODO: Define reward space according to scene
        self.reward_space = ScalarRewardSpace()
        
        self.done_size = 1
        
        self.last_observation = None
        self.last_reward = None
    
    def close(self):
        self.sock.close()
    
    def render(self):
        if not (self.last_observation is None):
            cv2.imshow('observation', self.last_observation)
            cv2.waitKey(1)
    
    def reset(self):
        # TODO: Reset the unity scene
        self.sock.send(self.action_space.reset())
        self.sock.send(self.action_space.noop())
        obs, reward, done, _ = self._recv_next_state()
        return obs

    def step(self, act, non_block=False):
        '''
        param act: action
        NOTE: Currently only support int action type
        TODO: More general action space
        '''
        sendData = self.action_space.pack(act)
    
        self.sock.send(sendData)
        
        if not non_block:
            return self._recv_next_state()

    def sample(self):
        return self.action_space.sample()

    def _recv_next_state(self):
        raw_data = recvall(self.sock, self.observation_space.size + self.reward_space.size + self.done_size)
        self.last_observation = self.observation_space.observe(raw_data[:self.observation_space.size])
        self.last_reward = self.reward_space.unpack(raw_data[self.observation_space.size:self.observation_space.size + self.reward_space.size])
        done = bool((raw_data[((self.observation_space.size + self.reward_space.size))]))
        
        return self.last_observation, self.last_reward, done, None

if __name__ == '__main__':
    env = Unity3DEnvironment()
    
    for episode in range(1000):
        obs = env.reset()
        for t in range(10000):
            #env.render()
            #act = env.sample() * 2.0
            act = np.array([2.0, 0.0])
            obs, reward, done, _ = env.step(act, non_block=False)
            print (obs.shape)
            print (act, reward, done)
        
            if done:
                break
        print ('Episode %d' % (episode))
    env.close()
