SelectRole_BtnReturn = {};
local this = SelectRole_BtnReturn;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.Start()
	
end

function this.OnClick()
	inst.Parent:OnCommand("Return");
end


