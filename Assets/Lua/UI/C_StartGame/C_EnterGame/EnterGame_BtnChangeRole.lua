EnterGame_BtnChangeRole = {};
local this = EnterGame_BtnChangeRole;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnClick()
	inst.Parent:OnCommand("ChangeRole");
end

