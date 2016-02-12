C_Login_C_StartGame = {};
local this = C_Login_C_StartGame;
local inst;


function this.Awake()
	inst = this.inst;
end
function  this.Cancel()
	UITools.ShowMsg("Exits Game!!!");
end

function this.Login()
	local username = inst:GetChild('c_username'):GetChild('inputFiled').Value
	local pwd = inst:GetChild('c_pwd'):GetChild('inputFiled').Value
	if not UITools.isValidString(username) or not UITools.isValidString(pwd) then
		UITools.ShowMsg('密码或者用户名输入不合法')
		return
	end
	--请求登录协议
	inst:CreateMsg():SetMsgType(MsgProtocol.Login):AddString(username):AddString(pwd):Send()
end
function this.OnReceiveData(data)
	local sessionKey = data:PopString()
	UITools.StoreSessionKey(sessionKey)
	UITools.ClosePanel(inst);
	UITools.ShowPanel(UITools.D('ServerSelect'))
end

function this.Register()
	UITools.Log("===========");
	UITools.ClosePanel(inst);
	UITools.ShowPanel(UITools.D('Register'))
end



