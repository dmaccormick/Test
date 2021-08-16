#pragma once

//Core Libraries
#include <Windows.h>
#include <Xinput.h>

class Controller
{
public:
	//--- Constructors and Destructor ---//
	Controller();
	Controller(int _id);
	~Controller();

	//--- Setters and Getters ---//
	bool getIsConnected();
	bool getButton(int _button);
	float getLeftStick_X();
	float getLeftStick_Y();
	float getRightStick_X();
	float getRightStick_Y();
	float getLeftTrigger();
	float getRightTrigger();

	//--- Methods ---//
	void update(float _dt);
	void vibrate(float _duration, float _leftStrength, float _rightStrength);

private:
	//--- Static Variables ---//
	static float DEADZONE_THUMBSTICK;
	static float DEADZONE_TRIGGER;

	//--- Private Data ---//
	XINPUT_STATE currentState;
	bool isConnected;
	int id;
	bool isVibrating;
	float timeLeftVibrating;

	//--- Utility Functions ---//
	void stopVibrating();
	float applyDeadzone(float _deadzone, float _value);
};