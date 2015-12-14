EnterGame = {};
local this = EnterGame;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.Start()
	
end


function  this.OnCommand(command , param)
	if command == "ChangeRole" then
		UITools.Log("3333");
		inst:OnCommand("MoveLeft");
		UITools.D("SelectRole"):OnCommand("MoveCenter");
	elseif command == "MoveLeft" then
		UITools.Log("1111");
		inst['pos'] = 'Left';
		UITools.TweenPos_X(inst,-780);
		UITools.Log("222");
	elseif command == "MoveCenter" then
		UITools.SA(inst,true);
		inst['pos'] = 'Center';
		UITools.TweenPos_X(inst,0);
	elseif command == "EndTween" then
		if inst['pos'] == 'Left' then
			UITools.SA(inst,false);
		end
	end
end

