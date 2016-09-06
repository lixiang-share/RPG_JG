C_Register_C_StartGame = {};
local this = C_Register_C_StartGame;
local inst;

function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst,false)
end

function this.Cancel()
	UITools.ClosePanel(inst);
	UITools.ShowPanel(UITools.D('Login'))
end

function this.Register()
	local username = inst:GetChild('c_username'):GetChild('inputFiled').Value
	local pwd = inst:GetChild('c_pwd'):GetChild('inputFiled').Value
	local pwdRepwat = inst:GetChild('c_pwdRepeat'):GetChild('inputFiled').Value
	local phoneNum = inst:GetChild('c_PhoneNum'):GetChild('inputFiled').Value
	
	if not UITools.isValidString(username) or  string.len(username) < 4  then
		UITools.ShowMsg('用户名长度不能少于4位')
	elseif not UITools.isValidString(pwd) or string.len(pwd) < 4 then 
		UITools.ShowMsg('密码长度不能少于4位')
	elseif pwd ~= pwdRepwat then
		UITools.ShowMsg('两次输入密码不一致')
	elseif UITools.isValidString(phoneNum) and string.len(phoneNum) ~= 11 then
		UITools.ShowMsg('手机号输入错误')
	else
		inst:CreateMsg():SetMsgType(MsgProtocol.Register):AddString(username):AddString(pwd):AddString(phoneNum):Send()
	end	
end

function this.OnReceiveData(data)
	inst:GetChild('c_username'):GetChild('inputFiled').Value = ''
	inst:GetChild('c_pwd'):GetChild('inputFiled').Value = ''
	inst:GetChild('c_pwdRepeat'):GetChild('inputFiled').Value = ''
	inst:GetChild('c_PhoneNum'):GetChild('inputFiled').Value = ''
	UITools.ClosePanel(inst);
	UITools.ShowPanel(UITools.D('Login'))
end