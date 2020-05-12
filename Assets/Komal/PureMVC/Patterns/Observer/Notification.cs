namespace komal.puremvc
{
    public class Notification: INotification
    {
        public Notification(string _name, object _body = null, string _type = null)
        {
            name = _name;
            body = _body;
            type = _type;
        }

        public override string ToString()
        {
            string msg = "Notification Name: " + name;
            msg += "\nBody:" + ((body == null) ? "null" : body.ToString());
            msg += "\nType:" + ((type == null) ? "null" : type);
            return msg;
        }

        public string name { get; }
        public object body { get; set; }
        public string type { get; set; }
    }
}
