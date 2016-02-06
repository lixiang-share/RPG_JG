Login_btnLogin = {};
local this = Login_btnLogin;
local inst;


function this.Awake()
	inst = Login_btnLogin.inst;
end

function this.OnClick()
	UITools.D('Login'):OnCommand('Login');
end


