C_selectItems = {};
local this = C_selectItems;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.Battle( )
	-- body
	UITools.D('FBView'):CallLuaMethod('Show')
end

function this.Backpack( )
	UITools.D('backpack'):CallLuaMethod('Show')
end

function this.Task()
	UITools.D('TaskInfo'):CallLuaMethod('Show')
end

function this.Skill()
	UITools.D('SkillInfo'):CallLuaMethod('Show')
end

function this.Store()
	-- body
end

function this.System()
	-- body
end

function this.Cancel()
	UITools.ClosePanel(inst)
end