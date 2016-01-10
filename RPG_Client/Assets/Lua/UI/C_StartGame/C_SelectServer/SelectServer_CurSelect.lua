SelectServer_CurSelect = {};
local this = SelectServer_CurSelect;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnClick()
	inst.Parent:OnCommand('SelectServer');
end

