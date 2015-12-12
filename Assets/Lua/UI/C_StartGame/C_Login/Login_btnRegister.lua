Login_btnRegister = {};
local this = Login_btnRegister;
local inst;


function this.Awake()
	inst = this.inst;
end



function this.OnClick()
	UITools.D('Login'):OnCommand('Register');
end


