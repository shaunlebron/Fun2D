require("colors.lua")
require("images.lua")
require("modes.lua")
require("timing.lua")
require("text.lua")
require("shapes.lua")
require("misc.lua")
require("game.lua")

function love.load()
	start()
end

function love.draw()
   _updateMode()
   _updateClocks()
end
