C_EnterGame_C_StartGame = {};
local this = C_EnterGame_C_StartGame;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst , false)
end
function this.OnEnable()
	if inst:G('All_Roles') == nil then
		inst:CreateMsg():SetMsgType(MsgProtocol.PreSelectHero):Send()
	end
end

function this.OnReceiveData( data )
	if data.MsgType == MsgProtocol.PreSelectHero then
		UITools.Set(inst:GetChild('c_notice'):GetChild('l_desc'),data:PopString())
		local list = UITools.MsgToRoleList(data)
		if list.Count == 0 then
			local r_boy = RoleItem()
			r_boy.Role_id = 'boy'
			r_boy.Name = 'Unname'
			r_boy.Level = 1
			r_boy.Gender = 0
			list:Add(r_boy)

			local r_girl = RoleItem()
			r_girl.Role_id = 'girl'
			r_girl.Name = 'Unname'
			r_girl.Level = 1
			r_girl.Gender = 1
			list:Add(r_girl)
		elseif list.Count == 1 then
			if list:get_Item(0).Role_id == 'boy' then
				local r_girl = RoleItem()
				r_girl.Role_id = 'girl'
				r_girl.Name = 'Unname'
				r_girl.Level = 1
				r_girl.Gender = 1
				list:Add(r_girl)
			else
				local r_boy = RoleItem()
				r_boy.Role_id = 'boy'
				r_boy.Name = 'Unname'
				r_boy.Level = 1
				r_boy.Gender = 0
				list:Add(r_boy)
			end
		end
		inst:S('All_Roles' , list)
		inst:S('curSelect' , list:get_Item(0))
		this.SelectRole(list:get_Item(0))
	elseif data.MsgType == MsgProtocol.EnterGame then
		UITools.ShowMsg('EnterGame')
		inst.GameMgr:LoadScene('NewPlayerCity')
	end
end

function this.EnterGame()
	UITools.Log('EnterGame')
	local role = inst:G('curSelect')
	local server = inst:G('curServer')
	inst:CreateMsg():SetMsgType(MsgProtocol.EnterGame):AddInt(server.Id):AddString(role.Role_id):AddString(role.Name):AddInt(role.Level):AddInt(role.Gender):Send()
end


function this.SelectRole( role)
	--UITools.Log('=======SelectRole==========')
	inst:S('curSelect' , role)
	if role.Role_id == 'boy' then
		UITools.SA(inst:GetChild('c_role_boy') , true)
		UITools.SA(inst:GetChild('c_role_girl') , false)
	else
		UITools.SA(inst:GetChild('c_role_boy') , false)
		UITools.SA(inst:GetChild('c_role_girl') , true)
	end
	UITools.Set(inst:GetChild('C_descInfo'):GetChild('l_name') , role.Name)
	UITools.Set(inst:GetChild('C_descInfo'):GetChild('l_level') , 'Lv.'..role.Level)

end

function  this.OnCommand(command , param)
	if command == "ChangeRole" then
		inst:OnCommand("MoveLeft");
		UITools.D("SelectRole"):OnCommand("MoveCenter");
	elseif command == "MoveLeft" then
		inst:S('pos','Left');
		UITools.TweenPos_X(inst,-780);
		UITools.SA(inst:GetChild('b_enter'),false);
	elseif command == "MoveCenter" then
		UITools.SA(inst,true);
		inst:S('pos','Center');
		UITools.TweenPos_X(inst,0);
		UITools.SA(inst:GetChild('b_enter'),true);
	elseif command == "EndTween" then
		if inst:G('pos') == 'Left' then
			UITools.SA(inst,false);
		end
	end
end

