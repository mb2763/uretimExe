using My.Entities.Models;
using System; 
namespace MyUI.MyControl
{
    public class OperasyonCardRowEventArgsV2 : EventArgs
    {
        public int Id { get; set; } = 0;
        public UretimTakipModelV2 Model { get; set; }
    }
    public class OperasyonCardManagerV2
    {

        public static int RowId { get; set; } = 0;

        public static event EventHandler<OperasyonCardRowEventArgsV2> SecildiEvent;
        public static void SecildiTetikle(object sender, OperasyonCardRowEventArgsV2 e)
        {
            SecildiEvent.Invoke(sender, e);
        }
    }
}