C_playerEquips = {};
local this = C_playerEquips;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst , false)
end
function this.OnEnable()
	
end

function  this.OnDisable( )
	
end

