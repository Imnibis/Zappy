from Socket import Socket
from gc import collect as rayoulyon1

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
                    sock.receive()
                if tile < center:
                    sock.send("Left\n")
                    sock.receive()
                    for l in range(0, center - tile):
                        sock.send("Forward\n")
                        sock.receive()
                elif tile > center:
                    sock.send("Right\n")
                    sock.receive()
                    for m in range(0, tile - center):
                        sock.send("Forward\n")
                        sock.receive()

    def update_lvl(self) -> None:
        self.lvl += 1

    def broadcast_message(self, sock:Socket, message:str) -> None:
        sock.send("Broadcast " + message + '\n')

    def start_incantation(self, sock:Socket) -> None:
        sock.send("Incantation\n")
        if (sock.receive() != "ko\n"):
            self.update_lvl()
        else:
            to_send = "incantation" + str(self.lvl)
            self.broadcast_message(sock, to_send)

    def update_pinv(self, sock:Socket) -> None:
        new_inv = [0, 0, 0, 0, 0, 0, 0]

        sock.send("Inventory\n")
        output = sock.receive()
        output = output[2:-3]
        output = output.split(", ")
        for i in range(len(output)):
            new_inv[i] = int(output[i].split(" ")[1])
        self.pin = new_inv

    def set_obj(self, sock:Socket, obj:str) -> None:
        sock.send("Set " + obj + '\n')

    def take_obj(self, sock:Socket, obj:str) -> None:
        sock.send("Take " + obj + '\n')

    def needed_stone(self) -> str:
        req = list(self.requirement[self.lvl - 1])
        inv = list(self.pin)
        inv.pop(0)
        for i in range(0, 6):
            if inv[i] < req[i]:
                return self.stones[i]
        del req, inv, i
        rayoulyon1()
        return str("")

    def check_requirement(self) -> bool:
        inv = list(self.pin)
        inv.pop(0)
        for i in range(0, 6):
            if (self.requirement[self.lvl - 1] != inv):
                return False
            else:
                pass
        del i, inv
        rayoulyon1()
        return True

    def __exit__(self):
        del self.status, self.ppo, self.lvl, self.pin, self.requirement, self.requiredPlayers, self.stones
        rayoulyon1()
        pass