from Socket import Socket

class Player():
    def __init__(self) -> None:
        self.status = True
        self.ppo = [0,  0]                      # player position: X Y O
        self.lvl = 1                            # player level
        self.pin = [0, 0, 0, 0, 0, 0, 0]        # player inventory
        self.requirement = [[1, 0, 0, 0, 0, 0], #required stone to initiate the elevation ritual
                            [1, 1, 1, 0, 0, 0],
                            [2, 0, 1, 0, 2, 0],
                            [1, 1, 2, 0, 1, 0],
                            [1, 2, 1, 3, 0, 0],
                            [1, 2, 3, 0, 1, 0],
                            [2, 2, 2, 2, 2, 1]]
        self.requiredPlayers = [0, 1, 2, 2, 4, 4, 6, 6]
        self.stones = ["linemate", "deraumere", "sibur", "mendiane", "phiras", "thystame"]

    def take_obj(self, sock:Socket, obj:str) -> None:
        sock.send("Take " + obj + '\n')
        print(sock.receive())

    def go_to(self, sock:Socket, tile:int) -> None:
        for i in range(1, 9):
            tmin = i**2
            tmax = tmin + (i*2)
            delta = (tmax - tmin) / 2
            center = tmin + delta
            center = int(center)
            if tmin < tile < tmax:
                for j in range(0, i):
                    sock.send("Forward\n")
                if tile < center:
                    sock.send("Left\n")
                    for l in range(0, center - tile):
                        sock.send("Forward\n")
                elif tile > center:
                    sock.send("Right\n")
                    for m in range(0, tile - center):
                        sock.send("Forward\n")

    def update_pinv(self, sock:Socket) -> None:
        sock.send("Inventory\n")
        output = sock.receive()
        output = output[2:-2]
        output = output.split(", ")
        for i in range(len(output)):
            new_out = output[i].split(" ")
            output[i] = new_out[1]
        for i in range(len(output)):
            self.pin[i] = int(output[i])

    def check_requirement(self) -> bool:
        for i in range(6):
            if (self.requirement[self.lvl][i] != self.pin[i + 1]):
                return False
            else:
                pass
        return True

    def broadcast_message(sock:Socket, message: str) -> None:
        sock.send("Broadcast " + message + '\n')

    def update_lvl(self) -> None:
        self.lvl += 1