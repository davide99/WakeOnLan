namespace WakeOnLan
{
    class Host
    {
        public string IP { get; }
        public string Hostname { get; set; }
        public string MAC { get; set; }

        public Host(string IP)
        {
            this.IP = IP;
        }
    }
}
