C_FBView = {};
local this = C_FBView;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst,false)
end
function this.OnEnable()
	
end

function  this.OnDisable( )
	
end
function this.Cancel()
	UITools.ClosePanel(inst)
end

function this.Show()
	UITools.ShowPanel(inst)
end

function this.Select(info)
	UITools.D("FBHint"):CallLuaMethod('Show')

end