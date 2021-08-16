//Core Libraries
#include <cmath>

//Our Headers
#include "Controller.h"

//--- Static Variables ---//
float Controller::DEADZONE_THUMBSTICK = 0.1f;
float Controller::DEADZONE_TRIGGER = 0.1f;



//--- Constructors and Destructor ---//
Controller::Controller()
{
	//Zero out the data to start with
	memset(&currentState, 0, sizeof(XINPUT_STATE));

	//Init the private data
	id = -1;
	isVibrating = false;
	isConnected = true;
	timeLeftVibrating = 0.0f;
}

Controller::Controller(int _id)
{
	//Zero out the data to start with
	memset(&currentState, 0, sizeof(XINPUT_STATE));

	//Store the index
	id = _id;
	isVibrating = false;
	isConnected = true;
	timeLeftVibrating = 0.0f;
}

Controller::~Controller()
{
	//Ensure there is no more vibration
	stopVibrating();

	//Zero out the data as clean up
	memset(&currentState, 0, sizeof(XINPUT_STATE));
}



//--- Setters and Getters ---//
bool Controller::getIsConnected()
{
	//Return if the controller is currently connected
	return isConnected;
}

bool Controller::getButton(int _button)
{
	switch (_button)
	{
		//Face Buttons ---
		case 0: //A button
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_A;
			break;

		case 1: //B button
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_B;
			break;

		case 2: //X button
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_X;
			break;

		case 3: //Y button
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_Y;
			break;


		//Bumpers ---
		case 4: //LB
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_LEFT_SHOULDER;
			break;

		case 5: //RB
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_RIGHT_SHOULDER;
			break;


		//Misc Buttons ---
		case 6: //Select
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_BACK;
			break;

		case 7: //Start
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_START;
			break;

		case 8: //LS
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_LEFT_THUMB;
			break;

		case 9: //RS
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_RIGHT_THUMB;
			break;


		//DPAD
		case 10: //Up
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_UP;
			break;

		case 11: //Right
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_RIGHT;
			break;

		case 12: //Down
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_DOWN;
			break;

		case 13: //Left
			return currentState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_LEFT;
			break;


		//Default
		default:
			return false;
	}
}

float Controller::getLeftStick_X()
{	
	//Get the x-value of the left stick, normalized from -1 to 1
	float lsX = (float)currentState.Gamepad.sThumbLX / 32767;
	
	//If below the deadzone, consider it 0. If above, scale accordingly
	if (abs(lsX) > DEADZONE_THUMBSTICK)
		return applyDeadzone(DEADZONE_THUMBSTICK, lsX);
	else
		return 0.0f;
}

float Controller::getLeftStick_Y()
{
	//Get the y-value of the left stick, normalized from -1 to 1
	float lsY = (float)currentState.Gamepad.sThumbLY / 32767;

	//If below the deadzone, consider it 0. If above, scale accordingly
	if (abs(lsY) > DEADZONE_THUMBSTICK)
		return applyDeadzone(DEADZONE_THUMBSTICK, lsY);
	else
		return 0.0f;
}

float Controller::getRightStick_X()
{
	//Get the x-value of the right stick, normalized from -1 to 1
	float rsX = (float)currentState.Gamepad.sThumbRX / 32767;

	//If below the deadzone, consider it 0. If above, scale accordingly
	if (abs(rsX) > DEADZONE_THUMBSTICK)
		return applyDeadzone(DEADZONE_THUMBSTICK, rsX);
	else
		return 0.0f;
}

float Controller::getRightStick_Y()
{
	//Get the y-value of the right stick, normalized from -1 to 1
	float rsY = (float)currentState.Gamepad.sThumbRY / 32767;

	//If below the deadzone, consider it 0. If above, scale accordingly
	if (abs(rsY) > DEADZONE_THUMBSTICK)
		return applyDeadzone(DEADZONE_THUMBSTICK, rsY);
	else
		return 0.0f;
}

float Controller::getLeftTrigger()
{
	//Get the value of the left trigger, normalized from 0 to 1
	float leftTrigger = (float)currentState.Gamepad.bLeftTrigger / 255;

	//If below the deadzone, consider it 0. If above, scale accordingly
	if (abs(leftTrigger) > DEADZONE_TRIGGER)
		return applyDeadzone(DEADZONE_TRIGGER, leftTrigger);
	else
		return 0.0f;
}

float Controller::getRightTrigger()
{
	//Get the value of the right trigger, normalized from 0 to 1
	float rightTrigger = (float)currentState.Gamepad.bRightTrigger / 255;

	//If below the deadzone, consider it 0. If above, scale accordingly
	if (abs(rightTrigger) > DEADZONE_TRIGGER)
		return applyDeadzone(DEADZONE_TRIGGER, rightTrigger);
	else
		return 0.0f;
}



//--- Methods ---//
void Controller::update(float _dt)
{
	//Get the new state data from XINPUT
	DWORD xinputResult = XInputGetState(id, &currentState);

	//Check if the controller is connected
	isConnected = (xinputResult == ERROR_SUCCESS);

	//If this controller is not connected, return immediately
	if (!isConnected)
		return;

	//If we are vibrating, decrement the time and stop vibrating if need be
	if (isVibrating)
	{
		//Decrement the time
		timeLeftVibrating -= _dt;

		//If out of time, stop the vibration
		if (timeLeftVibrating <= 0.0f)
			stopVibrating();
	}
}

void Controller::vibrate(float _duration, float _leftStrength, float _rightStrength)
{
	//If not connected, return immediately so we don't try to vibrate a controller that doesn't exist
	if (!isConnected)
		return;

	//Create a new XINPUT Vibration structure and zero it out
	XINPUT_VIBRATION vibrationAmounts;
	memset(&vibrationAmounts, 0, sizeof(XINPUT_VIBRATION));

	//Scale up to convert to XINPUT's numbers
	_leftStrength *= 65535.0f;
	_rightStrength *= 65535.0f;

	//Insert the vibration values into the structure
	vibrationAmounts.wLeftMotorSpeed = _leftStrength;
	vibrationAmounts.wRightMotorSpeed = _rightStrength;

	//Pass the vibration to XINPUT
	XInputSetState(id, &vibrationAmounts);

	//If the vibration amounts were not simply 0, start the countdown before vibration ends
	if (_leftStrength != 0.0f || _rightStrength != 0.0f)
	{
		//Start the vibration countdown
		isVibrating = true;

		//Set the duration of the vibration
		timeLeftVibrating = _duration;
	}
}



//--- Utility Functions ---//
void Controller::stopVibrating()
{
	//Set the time to 0 to ensure it isn't negative
	timeLeftVibrating = 0.0f;

	//No longer vibrating
	isVibrating = false;

	//Stop the motors from vibrating
	vibrate(0.0f, 0.0f, 0.0f);
}

float Controller::applyDeadzone(float _deadzone, float _value)
{
	//Store the sign of the value as either 1 or -1
	float sign = _value / abs(_value);

	//Cap the value from going above 1 or below -1
	if (abs(_value) > 1.0f)
		_value = sign;

	//Calculate how far above the deadzone we are
	float amountAboveDeadzone = abs(_value) - _deadzone;
	
	//Return the new value which is the % of the way between the deadzone and a full value (lerping, but with a final cancelled out equation)
	return sign * (amountAboveDeadzone / (1.0f - _deadzone));
}