_mode = nil

_fadeMin = 0
_fadeMed = 1
_fadeMax = 2

_fadeTime = _fadeMax
_fadeStep = (_fadeMed-_fadeMin) / 25
_fadeColor = "black"

_screenshot = nil

scene = function(m)
	_prevMode = _mode
	_mode = m
	_fadeTime = _fadeMin
	_screenshot = nil
end

fade = function(time, c)
	_fadeMin = 0
	_fadeMax = time
	_fadeMed = (_fadeMax - _fadeMin) / 2
	_fadeColor = c
end

_updateMode = function()
	local r,g,b
	local t
	t = love.timer.getTime()
	if _fadeColor ~= nil then
		r,g,b = _colorFromName(_fadeColor)
	end

	if _fadeTime < _fadeMed then
		color("white")
		if _screenshot ~= nil then
			love.graphics.draw(_screenshot, 0, 0)
		end
		local a = 1-(_fadeMed-_fadeTime)/(_fadeMed-_fadeMin)
		love.graphics.setColor(r,g,b,a*255)
		love.graphics.rectangle("fill",0,0,800,600)
	else
		_mode()
		if _screenshot == nil then
			_screenshot = love.graphics.newImage(love.graphics.newScreenshot())
		end

		if _fadeTime >= _fadeMed and _fadeTime < _fadeMax then
			local a = (_fadeMax-_fadeTime)/(_fadeMax-_fadeMed)
			love.graphics.setColor(r,g,b,a*255)
			love.graphics.rectangle("fill",0,0,800,600)
		end
	end

	if _fadeTime < _fadeMax then
		_fadeTime = _fadeTime + _fadeStep
	end

	while love.timer.getTime() - t < 1.0/20 do

	end
end