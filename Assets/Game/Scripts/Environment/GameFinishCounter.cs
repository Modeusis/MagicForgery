using TMPro;
using UnityEngine;

namespace Environment
{
    public class GameFinishCounter
    {
        private int _clientsToWin;
        private int _servedClients = 0;
        
        private TMP_Text _clientCounterText;
        
        public GameFinishCounter(int clientsToWin, TMP_Text clientsCounter)
        {
            _clientsToWin = clientsToWin;
            
            _clientCounterText = clientsCounter;
            
            RenewClientCounter();
        }
        
        public void ClientSuccess()
        {
            _servedClients++;

            RenewClientCounter();
            
            if (_servedClients == _clientsToWin)
            {
                ShowFinalScreen();
            }
        }

        public void ClientExpired()
        {
            if (_servedClients > 0)
            {
                _servedClients--;
            }
            
            RenewClientCounter();
        }
        
        private void RenewClientCounter()
        {
            _clientCounterText.text = $"{_servedClients}/{_clientsToWin}";
        }
        
        private void ShowFinalScreen()
        {
            Debug.Log("Game finished!");
        }
    }
}