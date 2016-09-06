C_FBHint = {};
local this = C_FBHint;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst,false)
end
function this.Cancel()
	UITools.ClosePanel(inst)
end

function this.Show(info)
	UITools.ShowPanel(inst)
end

function this.PersonalFight()
	UITools.BeginFight();
end

function this.TeamFight()
	UITools.ShowMsg("此功能暂未开放")
end