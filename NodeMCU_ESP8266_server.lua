-- Enter your wifi credentials
ssid = "ssid"
pass = "pass"
port = 180

-- Setup workPin
pin = 1
currentPinState = 0
receiveCommandText = "ON"

-- Init Pin
gpio.write(pin, gpio.HIGH)

-- Init wifi
wifi.setmode(wifi.STATION)
wifi.sta.config(ssid, pass)
wifi.sta.connect()

-- Create server: TCP, timeout 30s
sv=net.createServer(net.TCP, 30)

-- Function do receive, sck -- socket, data -- receive message
function receiver(sck, data)
    -- Write receive data
    print("Receive data: ", data)
    -- if receive command switch pin
    if data == receiveCommandText then
        if currentPinState == 1 then
            gpio.write(pin, gpio.LOW)
            currentPinState = 0
        else
            gpio.write(pin, gpio.HIGH)
            currentPinState = 1
        end

        print("Pin", pin, " has been switch")
    end
end

if sv then
  print("Server Create. IP:", wifi.sta.getip(), " Port: ", port)
  sv:listen(port, function(conn)
    print("Receive!")
    conn:on("receive", receiver)
  end)
end
