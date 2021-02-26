using System;

namespace  vanwall.ux
{
    public enum MessageType : byte
    {
        z_error = 0,

        WishToShare = 1,

        WantToShare = 2,

        NeedToShare = 3,

        ResponseAsRequested = 4
    }

    public static class UxCore
    {

        public static void ShareMessage(MessageType type, string message)
        {
            string typeString = type switch {
                MessageType.WishToShare => "[info] ",
                MessageType.WantToShare => "[warn] ",
                MessageType.NeedToShare => "[crit] ",
                MessageType.ResponseAsRequested => "",
                _ => "[unknown]"
            };

            DateTime snapshot = DateTime.Now;

            Console.WriteLine(
                string.Format(
                    "{0} {1:D2}:{2:D2}:{3:D2}> {4}{5}",
                    snapshot.ToShortDateString(),
                    snapshot.Hour,
                    snapshot.Minute,
                    snapshot.Second,
                    typeString,
                    message
                )
            );
        }

        public static void WaitForExit()
        {
            ShareMessage(MessageType.WishToShare, "use exit to exit");

            while(Console.ReadLine() != "exit")
            { }
        }
    }
}