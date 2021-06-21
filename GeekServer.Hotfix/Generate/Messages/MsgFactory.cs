//auto generated, do not modify it

namespace Geek.Server.Message
{
	public class MsgFactory
	{
		///<summary>通过msgId构造msg</summary>
		public static BaseMessage Create(int msgId)
		{
			switch(msgId)
			{
				//背包
				case 112001: return new Geek.Server.Message.DemoBag.ReqBagInfo();
				case 112002: return new Geek.Server.Message.DemoBag.ResBagInfo();
				case 112003: return new Geek.Server.Message.DemoBag.ReqUseItem();
				case 112004: return new Geek.Server.Message.DemoBag.ReqSellItem();
				case 112005: return new Geek.Server.Message.DemoBag.ResItemChange();
				
				//登陆
				case 111001: return new Geek.Server.Message.DemoLogin.ReqLogin();
				case 111002: return new Geek.Server.Message.DemoLogin.ResLogin();
				case 111003: return new Geek.Server.Message.DemoLogin.ResLevelUp();
				case 111004: return new Geek.Server.Message.DemoLogin.ResNotice();
				case 111005: return new Geek.Server.Message.DemoLogin.ReqChangeName();
				case 111006: return new Geek.Server.Message.DemoLogin.ResChangeName();
				
				//玩家快照
				case 101201: return new Geek.Server.Message.Login.ReqLogin();
				case 101202: return new Geek.Server.Message.Login.ReqReLogin();
				case 101101: return new Geek.Server.Message.Login.ResLogin();
				case 101102: return new Geek.Server.Message.Login.ResReLogin();
				case 101303: return new Geek.Server.Message.Login.HearBeat();
				case 101103: return new Geek.Server.Message.Login.ResPrompt();
				case 101104: return new Geek.Server.Message.Login.ResUnlockScreen();
				
				//举例各种结构写法
				case 111101: return new Geek.Server.Message.Sample.ReqTest();
				
				default: return default;
			}
		}
		
		public static T Create<T>(int msgId) where T : BaseMessage
		{
			return (T)Create(msgId);
		}
	}
}