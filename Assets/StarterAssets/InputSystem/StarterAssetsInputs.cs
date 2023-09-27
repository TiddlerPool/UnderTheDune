using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
        public bool sprint;
		public bool aim;
        public bool shoot;
		public bool next;
		public bool back;
		public bool leave;
		public bool interact;
		public bool inventory;
		public bool closeInventory;
        public bool note;
		public bool closeNote;
        public bool stop;
        public bool wakeUp;
        public bool trade;
        public bool use;
		public bool discard;
        public bool pause;
        public bool endPause;

        [Header("Movement Settings")]
		public bool analogMovement;
        public bool allowSprint;


		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
            if(allowSprint)
            {
                SprintInput(value.isPressed);
            }
		}

        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }

        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }

        public void OnNext(InputValue value)
        {
			NextInput(value.isPressed);
        }

		public void OnBack(InputValue value)
		{
			BackInput(value.isPressed);
		}

        public void OnLeave(InputValue value)
        {
            LeaveInput(value.isPressed);
        }

        public void OnInventory(InputValue value)
        {
            InventoryInput(value.isPressed);
        }

        public void OnCloseInventory(InputValue value)
        {
            CloseInventoryInput(value.isPressed);
        }

        public void OnNote(InputValue value)
        {
            NoteInput(value.isPressed);
        }

        public void OnCloseNote(InputValue value)
        {
            CloseNoteInput(value.isPressed);
        }

        public void OnStop(InputValue value)
        {
            StopInput(value.isPressed);
        }

        public void OnWakeUp(InputValue value)
        {
            WakeUpInput(value.isPressed);
        }

        public void OnTrade(InputValue value)
		{
			TradeInput(value.isPressed);
		}

        public void OnUse(InputValue value)
        {
            UseInput(value.isPressed);
        }

        public void OnDiscard(InputValue value)
        {
            DiscardInput(value.isPressed);
        }

        public void OnPause(InputValue value)
        {
            PauseInput(value.isPressed);
        }

        public void OnEndPause(InputValue value)
        {
            EndPauseInput(value.isPressed);
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

        public void ShootInput(bool newShootState)
        {
            shoot = newShootState;
        }

        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        public void NextInput(bool newNextState)
		{
			next = newNextState;
		}

		public void BackInput(bool newBackState)
		{
			back = newBackState;
		}

        public void LeaveInput(bool newLeaveState)
        {
            leave = newLeaveState;
        }

        public void InventoryInput(bool newInventoryState)
        {
            inventory = newInventoryState;
        }

        public void CloseInventoryInput(bool newCloseInventoryState)
        {
            closeInventory = newCloseInventoryState;
        }

        public void NoteInput(bool newNoteState)
        {
            note = newNoteState;
        }

        public void CloseNoteInput(bool newCloseNoteState)
        {
            closeNote = newCloseNoteState;
        }

        public void StopInput(bool newStopState)
        {
            stop = newStopState;
        }

        public void WakeUpInput(bool newWakeUpState)
        {
            wakeUp = newWakeUpState;
        }

        public void TradeInput(bool newTradeState)
		{
			trade = newTradeState;
		}

        public void UseInput(bool newUseState)
        {
            use = newUseState;
        }

        public void DiscardInput(bool newDiscardState)
        {
            discard = newDiscardState;
        }

        public void PauseInput(bool newPauseState)
        {
            pause = newPauseState;
        }

        public void EndPauseInput(bool newEndPauseState)
        {
            endPause = newEndPauseState;
        }

        /*
        private void OnApplicationFocus(bool hasFocus)
		{
			cursorInputForLook = false;
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		*/
    }
	
}