b_return_C_StartGame_C_SelectRole = {};
local this = b_return_C_StartGame_C_SelectRole;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.Start()
	
end

function this.OnClick()
	inst.Parent:OnCommand("Return");
end


