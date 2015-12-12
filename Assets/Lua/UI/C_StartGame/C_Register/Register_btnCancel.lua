Register_btnCancel = {};
local this = Register_btnCancel;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnClick()
	inst.Parent:OnCommand('Cancel');
end

function  this.OnCommand(command , param)

end

