b_changeRole_C_StartGame_C_EnterGame = {};
local this = b_changeRole_C_StartGame_C_EnterGame;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnClick()
	inst.Parent:OnCommand("ChangeRole");
end

