#pragma once

extern "C"
{
	//Set up all of the controllers, only call once
	__declspec(dllexport) void initializeControllers();

	//Update all of the controllers to get the new values and to stop the vibrations, call every frame
	__declspec(dllexport) void updateControllers(float _dt);

	//Vibrate the requested controller for a certain amount of time. Control each motor individually
	__declspec(dllexport) void vibrateController(int _id, float _duration, float _leftPower, float _rightPower);

	//Check if a button on the requested controller is up or down
	__declspec(dllexport) bool getButton(int _id, int _button);

	//Get the x-component of the left stick on a requested controller from -1 to 1
	__declspec(dllexport) float getLeftStick_X(int _id);

	//Get the y-component of the left stick on a requested controller from -1 to 1
	__declspec(dllexport) float getLeftStick_Y(int _id);

	//Get the x-component of the right stick on a requested controller from -1 to 1
	__declspec(dllexport) float getRightStick_X(int _id);

	//Get the y-component of the right stick on a requested controller from -1 to 1
	__declspec(dllexport) float getRightStick_Y(int _id);

	//Get the value of the left trigger on a requested controller from 0 - 1
	__declspec(dllexport) float getLeftTrigger(int _id);

	//Get the value of the right trigger on a requested controller from 0 - 1
	__declspec(dllexport) float getRightTrigger(int _id);

	//Check if the requested controller is currently connected
	__declspec(dllexport) bool getIsConnected(int _id);
}
