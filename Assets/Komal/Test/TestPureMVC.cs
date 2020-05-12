using UnityEngine;

namespace komal.test {
    public class TestPureMVC : puremvc.ComponentEx, puremvc.INotificationHandler
    {
        public void TestAll(){
            this.facade.RegisterCommand("MSG_TEST_PUREMVC", ()=> new TestCommand());
            this.facade.SendNotification("MSG_TEST_PUREMVC", 99);
        }

        public override string[] ListNotificationInterests()
        {
            return new string[] {
                "MSG_TESTCOMMAND_RESPONSE"
            };
        }

        public override void HandleNotification(puremvc.INotification notification)
        {
            switch (notification.name)
            {
                case "MSG_TESTCOMMAND_RESPONSE":
                    Debug.Log(">>>> HandleNotification: " + notification.ToString());
                    break;
            }
        }
    }

    class TestCommand: puremvc.SimpleCommand {
        public override void Execute(puremvc.INotification notification)
        {
            Debug.Log(">>>>> TestCommand Execute " + notification.ToString());
            this.SendNotification("MSG_TESTCOMMAND_RESPONSE");
        }
    }
}
