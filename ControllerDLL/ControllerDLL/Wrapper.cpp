//Core Libraries
#include <string>

//Our Headers
#include "Wrapper.h"
#include "Controller.h"

//XInput cannot hold more than 8 controllers, as an absolute maximum
const int MAX_NUM_CONTROLLERS = 8;
Controller controllers[MAX_NUM_CONTROLLERS];

void initializeControllers()
{
	//Loop through all of the controllers and set them up with a unique id
	for (int i = 0; i < MAX_NUM_CONTROLLERS; i++)
		controllers[i] = Controller(i);
}

void updateControllers(float _dt)
{
	//Update all of the controllers so their inputs are accurate. Controller.update() automatically checks if the controller is disconnected
	for (int i = 0; i < MAX_NUM_CONTROLLERS; i++)
		controllers[i].update(_dt);
}

void vibrateController(int _id, float _duration, float _leftPower, float _rightPower)
{
	//Ensure the ID isn't out of range. If it is, return immediately
	if (_id >= MAX_NUM_CONTROLLERS)
		return;

	//Vibrate the requested controller with the given time and strengths
	controllers[_id].vibrate(_duration, _leftPower, _rightPower);
}

bool getButton(int _id, int _button)
{
	//Ensure the ID isn't out of range. If it is, return immediately
	if (_id >= MAX_NUM_CONTROLLERS)
		return false;

	return controllers[_id].getButton(_button);
}

float getLeftStick_X(int _id)
{
	//Ensure the ID isn't out of range. If it is, return immediately
	if (_id >= MAX_NUM_CONTROLLERS)
		return false;

	return controllers[_id].getLeftStick_X();
}

float getLeftStick_Y(int _id)
{
	//Ensure the ID isn't out of range. If it is, return immediately
	if (_id >= MAX_NUM_CONTROLLERS)
		return false;

	return controllers[_id].getLeftStick_Y();
}

float getRightStick_X(int _id)
{
	//Ensure the ID isn't out of range. If it is, return immediately
	if (_id >= MAX_NUM_CONTROLLERS)
		return false;

	return controllers[_id].getRightStick_X();
}

float getRightStick_Y(int _id)
{
	//Ensure the ID isn't out of range. If it is, return immediately
	if (_id >= MAX_NUM_CONTROLLERS)
		return false;

	return controllers[_id].getRightStick_Y();
}

float getLeftTrigger(int _id)
{
	//Ensure the ID isn't out of range. If it is, return immediately
	if (_id >= MAX_NUM_CONTROLLERS)
		return false;

	return controllers[_id].getLeftTrigger();
}

float getRightTrigger(int _id)
{
	//Ensure the ID isn't out of range. If it is, return immediately
	if (_id >= MAX_NUM_CONTROLLERS)
		return false;

	return controllers[_id].getRightTrigger();
}

bool getIsConnected(int _id)
{
	//Ensure the ID isn't out of range. If it is, return immediately
	if (_id >= MAX_NUM_CONTROLLERS)
		return false;

	return controllers[_id].getIsConnected();
}