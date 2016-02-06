Register_btnRegister = {};
local this = Register_btnRegister;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnClick()
	inst.Parent:OnCommand('Register');
end

function  this.OnCommand(command , param)

end

