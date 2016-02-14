C_SelectRole_C_StartGame = {};
local this = C_SelectRole_C_StartGame;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst , false)
end

function this.OnEnable()
	--inst:S('curSelect' , 'none')
end

function this.SelectRole(name)
	local roles = UITools.D('EnterGame'):G('All_Roles')
	for i=0,roles.Count - 1 do
		if roles:get_Item(i).Role_id == name then
			inst:S('curSelect' , roles:get_Item(i))
		end
	end
	inst:GetChild('input_name'):GetChild('inputFiled').Value = inst:G('curSelect').Name
	if name == 'boy' then
		UITools.Tween_Scale(inst:GetChild('c_role_boy') , 1.2,1.2)
		UITools.Tween_Scale(inst:GetChild('c_role_girl') , 1,1)
	elseif name == 'girl' then
		UITools.Tween_Scale(inst:GetChild('c_role_boy') , 1,1)
		UITools.Tween_Scale(inst:GetChild('c_role_girl') , 1.2,1.2)
	end
end

function this.ChangeName()
	local text = inst:GetChild('input_name'):GetChild('inputFiled').Value
	if inst:G('curSelect') ~= nil and inst:G('curSelect') ~= 'none' and UITools.isValidString(text) then
		inst:G('curSelect').Name =  text
		UITools.D('EnterGame'):CallLuaMethod('SelectRole',inst:G('curSelect'))
	end
end


function  this.OnCommand(command , param)
	if command == "Return" then
		inst:OnCommand("Right");
		UITools.D("EnterGame"):OnCommand("MoveCenter");

	elseif command == "Right" then
		inst:S('pos', 'Right');
		UITools.TweenPos_X(inst,836);

	elseif command == "MoveCenter" then
		UITools.SA(inst,true);
		inst:S('pos','Center');
		UITools.TweenPos_X(inst,0);
	elseif command == "EndTween" then
		if inst:G('pos') == 'Right' then
			UITools.SA(inst,false);
		end
	end
end


