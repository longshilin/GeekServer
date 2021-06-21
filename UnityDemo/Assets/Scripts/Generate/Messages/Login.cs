//auto generated, do not modify it
//限制：命名不能以下划线结尾(可能冲突)
//限制：不能跨协议文件继承,不能跨文件使用继承关系
//限制：map的key只支持short, int, long, string；list/map不能optional,list/map不能嵌套
//兼容限制：字段只能添加，添加后不能删除，添加字段只能添加到最后,添加消息类型只能添加到最后
//兼容限制：不能修改字段类型（如从bool改为long）
//兼容限制：消息类型(含msdId)不能作为其他消息的成员类型
using System.Collections.Generic;

///<summary>玩家快照</summary>
namespace Geek.Client.Message.Login
{
	internal class LoginMsgFactory
	{
		///<summary>通过msgIdx构造msg</summary>
		public static BaseMessage Create(int msgIdx)
		{
			switch(msgIdx)
			{
				case 1: return new RoleInfo();
				case 2: return new ReqLogin();
				case 3: return new ReqReLogin();
				case 4: return new ResLogin();
				case 5: return new ResReLogin();
				case 6: return new HearBeat();
				case 7: return new ResPrompt();
				case 8: return new ResUnlockScreen();
				default: return default;
			}
		}
	}
	
	
	///<summary>玩家信息</summary>
    public class RoleInfo : BaseMessage
	{
		internal virtual byte _msgIdx_ => 1;//最多支持255个消息类型
		
		///<summary>id</summary>
		public long roleId { get; set;}

		///<summary>名字</summary>
		public string roleName { get; set;}

		///<summary>角色等级</summary>
		public int level { get; set;}

		///<summary>vip等级</summary>
		public int vipLevel { get; set;}

		///<summary>战斗力</summary>
		public long fightPower { get; set;}

		///<summary>公会id</summary>
		public long guildId { get; set;}

		///<summary>公会名</summary>
		public string guildName { get; set;}

		///<summary>开服天数</summary>
		public int openServerDays { get; set;}

		///<summary>世界等级</summary>
		public int serverLevel { get; set;}

		///<summary>登陆时间</summary>
		public long loginTick { get; set;}

		///<summary>创角时间</summary>
		public long createTick { get; set;}

		///<summary>知否是gm玩家</summary>
		public bool isGMRole { get; set;}


		
		///<summary>反序列化，读取数据</summary>
        public override int Read(byte[] _buffer_, int _offset_)
		{
			_offset_ = base.Read(_buffer_, _offset_);
			int _startOffset_ = _offset_;
			int _toReadLength_ = XBuffer.ReadInt(_buffer_, ref _offset_);
			
			//字段个数,最多支持255个
			var _fieldNum_ = XBuffer.ReadByte(_buffer_, ref _offset_);
			
			do {
				if(_fieldNum_ > 0){
					roleId = XBuffer.ReadLong(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 1){
					roleName = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 2){
					level = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 3){
					vipLevel = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 4){
					fightPower = XBuffer.ReadLong(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 5){
					guildId = XBuffer.ReadLong(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 6){
					guildName = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 7){
					openServerDays = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 8){
					serverLevel = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 9){
					loginTick = XBuffer.ReadLong(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 10){
					createTick = XBuffer.ReadLong(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 11){
					isGMRole = XBuffer.ReadBool(_buffer_, ref _offset_);
					
				}else break;
			}while(false);
			
			//剔除多余数据
			if(_offset_ < _toReadLength_ - _startOffset_)
				_offset_ += _toReadLength_ - _startOffset_;
			return _offset_;
		}
		
		///<summary>序列化，写入数据</summary>
        public override int Write(byte[] _buffer_, int _offset_)
        {	
			_offset_ = base.Write(_buffer_, _offset_);
			//先写入当前对象长度占位符
			int _startOffset_ = _offset_;
			XBuffer.WriteInt(0, _buffer_, ref _offset_);
			
			//写入字段数量,最多支持255个
			XBuffer.WriteByte(12, _buffer_, ref _offset_);
			
			//写入数据
			XBuffer.WriteLong(roleId, _buffer_, ref _offset_);
			XBuffer.WriteString(roleName, _buffer_, ref _offset_);
			XBuffer.WriteInt(level, _buffer_, ref _offset_);
			XBuffer.WriteInt(vipLevel, _buffer_, ref _offset_);
			XBuffer.WriteLong(fightPower, _buffer_, ref _offset_);
			XBuffer.WriteLong(guildId, _buffer_, ref _offset_);
			XBuffer.WriteString(guildName, _buffer_, ref _offset_);
			XBuffer.WriteInt(openServerDays, _buffer_, ref _offset_);
			XBuffer.WriteInt(serverLevel, _buffer_, ref _offset_);
			XBuffer.WriteLong(loginTick, _buffer_, ref _offset_);
			XBuffer.WriteLong(createTick, _buffer_, ref _offset_);
			XBuffer.WriteBool(isGMRole, _buffer_, ref _offset_);
			
			//覆盖当前对象长度
			XBuffer.WriteInt(_offset_ - _startOffset_, _buffer_, ref _startOffset_);
			return _offset_;
		}
	}

	///<summary>登陆</summary>
    public class ReqLogin : BaseMessage
	{
		internal virtual byte _msgIdx_ => 2;//最多支持255个消息类型
        public override int GetMsgId() { return MsgId; }
        public const int MsgId = 101201;
		
		///<summary>登陆用户名</summary>
		public string userName { get; set;}

		///<summary>游戏服务器Id</summary>
		public int serverId { get; set;}

		///<summary>sdk登陆标识</summary>
		public string sdkToken { get; set;}

		///<summary>sdk类型 0无sdk</summary>
		public int sdkType { get; set;}

		///<summary>渠道id</summary>
		public string channelId { get; set;}

		///<summary>是否为后台重连</summary>
		public bool isRelogin { get; set;}

		///<summary>登陆token,客户端启动游戏生成一次[相同代表是同一重连/不同则顶号]</summary>
		public long handToken { get; set;}

		///<summary>0编辑器，1android, 2ios, 3ios越狱</summary>
		public int deviceType { get; set;}

		///<summary>手机系统 android ios</summary>
		public string deviceOS { get; set;}

		///<summary>设备型号</summary>
		public string deviceModel { get; set;}

		///<summary>设备名字</summary>
		public string deviceName { get; set;}

		///<summary>设备id</summary>
		public string deviceId { get; set;}


		
		///<summary>反序列化，读取数据</summary>
        public override int Read(byte[] _buffer_, int _offset_)
		{
			_offset_ = base.Read(_buffer_, _offset_);
			
			//字段个数,最多支持255个
			var _fieldNum_ = XBuffer.ReadByte(_buffer_, ref _offset_);
			
			do {
				if(_fieldNum_ > 0){
					userName = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 1){
					serverId = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 2){
					sdkToken = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 3){
					sdkType = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 4){
					channelId = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 5){
					isRelogin = XBuffer.ReadBool(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 6){
					handToken = XBuffer.ReadLong(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 7){
					deviceType = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 8){
					deviceOS = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 9){
					deviceModel = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 10){
					deviceName = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 11){
					deviceId = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
			}while(false);
			
			return _offset_;
		}
		
		///<summary>序列化，写入数据</summary>
        public override int Write(byte[] _buffer_, int _offset_)
        {	
			_offset_ = base.Write(_buffer_, _offset_);
			
			//写入字段数量,最多支持255个
			XBuffer.WriteByte(12, _buffer_, ref _offset_);
			
			//写入数据
			XBuffer.WriteString(userName, _buffer_, ref _offset_);
			XBuffer.WriteInt(serverId, _buffer_, ref _offset_);
			XBuffer.WriteString(sdkToken, _buffer_, ref _offset_);
			XBuffer.WriteInt(sdkType, _buffer_, ref _offset_);
			XBuffer.WriteString(channelId, _buffer_, ref _offset_);
			XBuffer.WriteBool(isRelogin, _buffer_, ref _offset_);
			XBuffer.WriteLong(handToken, _buffer_, ref _offset_);
			XBuffer.WriteInt(deviceType, _buffer_, ref _offset_);
			XBuffer.WriteString(deviceOS, _buffer_, ref _offset_);
			XBuffer.WriteString(deviceModel, _buffer_, ref _offset_);
			XBuffer.WriteString(deviceName, _buffer_, ref _offset_);
			XBuffer.WriteString(deviceId, _buffer_, ref _offset_);
			
			return _offset_;
		}
	}

	///<summary>请求重连</summary>
    public class ReqReLogin : BaseMessage
	{
		internal virtual byte _msgIdx_ => 3;//最多支持255个消息类型
        public override int GetMsgId() { return MsgId; }
        public const int MsgId = 101202;
		
		public string sdkToken { get; set;}

		public long handToken { get; set;}


		
		///<summary>反序列化，读取数据</summary>
        public override int Read(byte[] _buffer_, int _offset_)
		{
			_offset_ = base.Read(_buffer_, _offset_);
			
			//字段个数,最多支持255个
			var _fieldNum_ = XBuffer.ReadByte(_buffer_, ref _offset_);
			
			do {
				if(_fieldNum_ > 0){
					sdkToken = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 1){
					handToken = XBuffer.ReadLong(_buffer_, ref _offset_);
					
				}else break;
			}while(false);
			
			return _offset_;
		}
		
		///<summary>序列化，写入数据</summary>
        public override int Write(byte[] _buffer_, int _offset_)
        {	
			_offset_ = base.Write(_buffer_, _offset_);
			
			//写入字段数量,最多支持255个
			XBuffer.WriteByte(2, _buffer_, ref _offset_);
			
			//写入数据
			XBuffer.WriteString(sdkToken, _buffer_, ref _offset_);
			XBuffer.WriteLong(handToken, _buffer_, ref _offset_);
			
			return _offset_;
		}
	}

	///<summary>登陆结果</summary>
    public class ResLogin : BaseMessage
	{
		internal virtual byte _msgIdx_ => 4;//最多支持255个消息类型
        public override int GetMsgId() { return MsgId; }
        public const int MsgId = 101101;
		
		///<summary>登陆结果1成功，其他失败</summary>
		public int result { get; set;}

		///<summary>登陆失败的原因</summary>
		public int reason { get; set;}

		///<summary>角色信息</summary>
		public RoleInfo role{ get{ return _role_; } set{ _role_ = value;  hasRole = value != default; }}
		RoleInfo _role_;
		public bool hasRole { get; private set; }

		///<summary>登陆用户名</summary>
		public string userName { get; set;}

		///<summary>是否为新角色</summary>
		public bool isNewCreate { get; set;}


		
		///<summary>反序列化，读取数据</summary>
        public override int Read(byte[] _buffer_, int _offset_)
		{
			_offset_ = base.Read(_buffer_, _offset_);
			
			//字段个数,最多支持255个
			var _fieldNum_ = XBuffer.ReadByte(_buffer_, ref _offset_);
			
			do {
				if(_fieldNum_ > 0){
					result = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 1){
					reason = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 2){
					hasRole = XBuffer.ReadBool(_buffer_, ref _offset_);
					if(hasRole){
					var _idx_ = XBuffer.ReadByte(_buffer_, ref _offset_);
					role = new RoleInfo();
					_offset_ = role.Read(_buffer_, _offset_);
					}
				}else break;
				if(_fieldNum_ > 3){
					userName = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 4){
					isNewCreate = XBuffer.ReadBool(_buffer_, ref _offset_);
					
				}else break;
			}while(false);
			
			return _offset_;
		}
		
		///<summary>序列化，写入数据</summary>
        public override int Write(byte[] _buffer_, int _offset_)
        {	
			_offset_ = base.Write(_buffer_, _offset_);
			
			//写入字段数量,最多支持255个
			XBuffer.WriteByte(5, _buffer_, ref _offset_);
			
			//写入数据
			XBuffer.WriteInt(result, _buffer_, ref _offset_);
			XBuffer.WriteInt(reason, _buffer_, ref _offset_);
			XBuffer.WriteBool(hasRole, _buffer_, ref _offset_);
			if(hasRole)
			{
				XBuffer.WriteByte(role._msgIdx_, _buffer_, ref _offset_);
				_offset_ = role.Write(_buffer_, _offset_);
			}
				
			XBuffer.WriteString(userName, _buffer_, ref _offset_);
			XBuffer.WriteBool(isNewCreate, _buffer_, ref _offset_);
			
			return _offset_;
		}
	}

	///<summary>断线重连</summary>
    public class ResReLogin : BaseMessage
	{
		internal virtual byte _msgIdx_ => 5;//最多支持255个消息类型
        public override int GetMsgId() { return MsgId; }
        public const int MsgId = 101102;
		
		///<summary>重连结果</summary>
		public bool success { get; set;}


		
		///<summary>反序列化，读取数据</summary>
        public override int Read(byte[] _buffer_, int _offset_)
		{
			_offset_ = base.Read(_buffer_, _offset_);
			
			//字段个数,最多支持255个
			var _fieldNum_ = XBuffer.ReadByte(_buffer_, ref _offset_);
			
			do {
				if(_fieldNum_ > 0){
					success = XBuffer.ReadBool(_buffer_, ref _offset_);
					
				}else break;
			}while(false);
			
			return _offset_;
		}
		
		///<summary>序列化，写入数据</summary>
        public override int Write(byte[] _buffer_, int _offset_)
        {	
			_offset_ = base.Write(_buffer_, _offset_);
			
			//写入字段数量,最多支持255个
			XBuffer.WriteByte(1, _buffer_, ref _offset_);
			
			//写入数据
			XBuffer.WriteBool(success, _buffer_, ref _offset_);
			
			return _offset_;
		}
	}

	///<summary>双向心跳/收到恢复同样的消息</summary>
    public class HearBeat : BaseMessage
	{
		internal virtual byte _msgIdx_ => 6;//最多支持255个消息类型
        public override int GetMsgId() { return MsgId; }
        public const int MsgId = 101303;
		
		///<summary>当前时间</summary>
		public long timeTick { get; set;}


		
		///<summary>反序列化，读取数据</summary>
        public override int Read(byte[] _buffer_, int _offset_)
		{
			_offset_ = base.Read(_buffer_, _offset_);
			
			//字段个数,最多支持255个
			var _fieldNum_ = XBuffer.ReadByte(_buffer_, ref _offset_);
			
			do {
				if(_fieldNum_ > 0){
					timeTick = XBuffer.ReadLong(_buffer_, ref _offset_);
					
				}else break;
			}while(false);
			
			return _offset_;
		}
		
		///<summary>序列化，写入数据</summary>
        public override int Write(byte[] _buffer_, int _offset_)
        {	
			_offset_ = base.Write(_buffer_, _offset_);
			
			//写入字段数量,最多支持255个
			XBuffer.WriteByte(1, _buffer_, ref _offset_);
			
			//写入数据
			XBuffer.WriteLong(timeTick, _buffer_, ref _offset_);
			
			return _offset_;
		}
	}

	///<summary>服务器通知</summary>
    public class ResPrompt : BaseMessage
	{
		internal virtual byte _msgIdx_ => 7;//最多支持255个消息类型
        public override int GetMsgId() { return MsgId; }
        public const int MsgId = 101103;
		
		///<summary>通知内容</summary>
		public string msg { get; set;}

		///<summary>通知内容语言包id</summary>
		public int msgLanId { get; set;}

		///<summary>1tip, 2弹窗提示 3弹窗回到登陆，4弹窗退出游戏</summary>
		public short type { get; set;}


		
		///<summary>反序列化，读取数据</summary>
        public override int Read(byte[] _buffer_, int _offset_)
		{
			_offset_ = base.Read(_buffer_, _offset_);
			
			//字段个数,最多支持255个
			var _fieldNum_ = XBuffer.ReadByte(_buffer_, ref _offset_);
			
			do {
				if(_fieldNum_ > 0){
					msg = XBuffer.ReadString(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 1){
					msgLanId = XBuffer.ReadInt(_buffer_, ref _offset_);
					
				}else break;
				if(_fieldNum_ > 2){
					type = XBuffer.ReadShort(_buffer_, ref _offset_);
					
				}else break;
			}while(false);
			
			return _offset_;
		}
		
		///<summary>序列化，写入数据</summary>
        public override int Write(byte[] _buffer_, int _offset_)
        {	
			_offset_ = base.Write(_buffer_, _offset_);
			
			//写入字段数量,最多支持255个
			XBuffer.WriteByte(3, _buffer_, ref _offset_);
			
			//写入数据
			XBuffer.WriteString(msg, _buffer_, ref _offset_);
			XBuffer.WriteInt(msgLanId, _buffer_, ref _offset_);
			XBuffer.WriteShort(type, _buffer_, ref _offset_);
			
			return _offset_;
		}
	}

	///<summary>解屏消息</summary>
    public class ResUnlockScreen : BaseMessage
	{
		internal virtual byte _msgIdx_ => 8;//最多支持255个消息类型
        public override int GetMsgId() { return MsgId; }
        public const int MsgId = 101104;
		

		
		///<summary>反序列化，读取数据</summary>
        public override int Read(byte[] _buffer_, int _offset_)
		{
			_offset_ = base.Read(_buffer_, _offset_);
			
			//字段个数,最多支持255个
			var _fieldNum_ = XBuffer.ReadByte(_buffer_, ref _offset_);
			
			do {
			}while(false);
			
			return _offset_;
		}
		
		///<summary>序列化，写入数据</summary>
        public override int Write(byte[] _buffer_, int _offset_)
        {	
			_offset_ = base.Write(_buffer_, _offset_);
			
			//写入字段数量,最多支持255个
			XBuffer.WriteByte(0, _buffer_, ref _offset_);
			
			//写入数据
			
			return _offset_;
		}
	}
}