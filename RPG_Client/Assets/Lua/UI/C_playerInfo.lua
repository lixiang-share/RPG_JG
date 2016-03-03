C_playerInfo = {};
local this = C_playerInfo;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()

end
function this.OnEnable()
	
end

function  this.OnDisable( )
	
end

function this.ShowDetails( )
	UITools.Log('ShowDetails')
	UITools.D('PlayerDetails'):CallLuaMethod('Show')
end

