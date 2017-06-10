using System;
using LF.Schedule.Contract;

namespace LF.Schedule.Task.Contract
{
  public  class ManagerService:IManagerService
    {
        public StateListResult GetStateList()
        {
            throw new NotImplementedException();
        }

        public SendCommandResult SendCommand(SendCommandParam sendCommandParam)
        {
            throw new NotImplementedException();
        }
    }
}
