import socket
import sys
import struct
import random
import numpy as np
import cv2

from operator import mul

SIZE_OF_UNITY_PIXEL_CHANNEL = 4

def image_rgba32_to_bgr(data):
	data = (data[:,:,:3] * 255.0).astype(np.uint8)
	data = cv2.cvtColor(data, cv2.COLOR_RGB2BGR)
	return data
	
def image_unity3d_to_cv2(data):
	data = image_rgba32_to_bgr(data)
	data = np.flip(data, axis=0)
	return data

class DiscreteActionSpace(object):
	def __init__(self, num_action):
		self.n = num_action
	def sample(self):
		return random.randint(0, self.n)
		
class ObservationSpace(object):
	def observe(self, raw_data):
		raise NotImplementedError()
		
class VisionObservationSpace(ObservationSpace):
	def __init__(self, shape):
		self.shape = shape
		self.size = reduce(mul, shape) * SIZE_OF_UNITY_PIXEL_CHANNEL
		
	def observe(self, raw_data):
		observation = np.fromstring(raw_data, np.float32)
		observation = observation.reshape(self.shape)
		observation = image_unity3d_to_cv2(observation)
		return observation
	
class Unity3DEnvironment(object):
	def __init__(self, server_address=('127.0.0.1', 8888), scene='test'):
		# Create a TCP/IP socket
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		
		# Connect the socket to the port where the server is listening
		self.server_address = server_address
		self.sock.connect(server_address)
		
		# TODO: Scene creation schemea
		
		# TODO: Define action space according to scene
		# Now testing default size=6 action space
		self.action_space = DiscreteActionSpace(6)
		
		# TODO: Define observation space according to scene
		# Now testing default size=256*256*4 observation space
		self.observation_space = VisionObservationSpace((256, 256, 4))
		
		self.last_observation = None
	
	def close(self):
		self.sock.close()
	
	def render(self):
		if not (self.last_observation is None):
			cv2.imshow('observation', self.last_observation)
			cv2.waitKey(1)
	
	def reset(self):
		# TODO: Reset the unity scene
		pass
	
	def step(self, act):
		'''
		param act: action
		NOTE: Currently only support int action type
		TODO: More general action space
		'''
		sendData = struct.pack("I", act)
		self.sock.send(sendData)
		
		raw_data = self.sock.recv(self.observation_space.size)
		self.last_observation = self.observation_space.observe(raw_data)
		
		return self.last_observation
	
	def sample(self):
		return self.action_space.sample()
		
if __name__ == '__main__':
	env = Unity3DEnvironment()
	
	env.reset()
	
	for t in xrange(100):
		env.step(env.sample())
		env.render()
	env.close()