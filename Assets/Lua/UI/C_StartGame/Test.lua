Test = {};
local this = Test;



function this.Awake()

	UITools.Log("Awake--");
end

function this.Start()
	UITools.LogError("Start--");
end


function this.OnEnable()
	UITools.LogWarning("OnEnable--");
end

function this.OnClick()
	print("OnClick");
end


