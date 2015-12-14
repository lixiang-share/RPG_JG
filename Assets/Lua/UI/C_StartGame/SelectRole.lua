SelectRole = {};
local this = SelectRole;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.Start()
	
end



function  this.OnCommand(command , param)
	if command == "Return" then
		UITools.Log("3333");

		inst:OnCommand(inst,"Right");
		UITools.D("SelectRole"):OnCommand("MoveCenter");

	elseif command == "Right" then
		UITools.Log("1111");
		inst['pos'] = 'Right';
		UITools.TweenPos_X(inst,836);
		UITools.Log("222");

	elseif command == "MoveCenter" then
		UITools.SA(inst,true);
		inst['pos'] = 'Center';
		UITools.TweenPos_X(inst,0);
	elseif command == "EndTween" then
		if inst['pos'] == 'Right' then
			UITools.SA(inst,false);
		end
	end
end


