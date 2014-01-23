_clocks = {}

startclock = function(name)
	if _clocks[name] == nil then
		_clocks[name] = {}
	end

	_clocks[name].time = 0
	_clocks[name].active = true
end

clock = function(name)
	return _formatfloat(_clocks[name].time)
end

stopclock = function(name)
	_clocks[name].active = false
end

_updateClocks = function()
	local d = love.timer.getDelta()
	for i,v in pairs(_clocks) do
		if v.active then
			v.time = v.time + d
		end
	end
end

_formatfloat = function(n)
	return math.floor(n*100) / 100
end