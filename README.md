# What is this?

This repository is a test of Unity's [ML-Agent](https://github.com/Unity-Technologies/ml-agents) plugin. In a nutshell, ML-Agents plugin lets you train AI using machine learning. The magic of intelligence happens in two steps.

In the first step you set-up an environment in which an AI body needs to figure out how to solve a _situation_. The situation is defined by a _reward_ function. Roughly speaking, the reward function is positive when the AI body is doing well, it is negative when is doing badly. This environment is then used by an external algorithm to train. Hopefully the algorithm converges after some iterations. The result is a .bytes files that contains the pure essence of intelligence.

In the second step you implant the intelligence (.bytes file) in the AI body. After that, the AI body performs in a near-optimal way to solve the situation.

 
# Caterpillar Movement

To understand how the ML-Agents plugin works I created the following problem:

	What's the optimal way for a caterpillar to move?
	
I was expecting the AI to discover this:

![](http://i.imgur.com/D0rydqJ.gif)

So I created a model for a caterpillar with a single joint in the middle joining two points on the ground. This is how the caterpillar model moves when using random inputs:

![](walking-ai-screenshots/CaterpillarTraining.gif)

After 250k iterations of the [PPO algorithm](https://arxiv.org/abs/1707.06347) that Unity provides out-of-the-box, the caterpillar learnt to move like this:

![](walking-ai-screenshots/CaterpillarTraining-ZoomingForward.gif)

# Notes

In the code and in other Unity assets I may refer to "caterpillar" as "slug". That's due to English not being my first language and me being lazy (sorry!).

