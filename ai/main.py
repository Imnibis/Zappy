#!/usr/bin/env python3
from sys import argv as av
from sys import stderr, stdout
from sys import exit as xit
import socket

class Map():
    def __init__(self) -> None:
        self.msz = [0, 0]                # map size: X Y

class Player():
    def __init__(self) -> None:
        self.pn = 0
        self.ppo = [0,  0]               # player position: X Y O
        self.plv = 1                     # player level
        self.pin = [0, 0, 0, 0, 0, 0, 0] # player inventory
        self.tna =  list()               # name of all the teams

class Socket():
    def __init__(self) -> None:
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    def connect(self, host:str, port:int) -> None:
        try:
            self.sock.connect((host, port))
        except ConnectionRefusedError:
            stderr.write("ConnectionRefusedError: can't connect to server\n")
            xit(84)

    def send(self, msg:str) -> None:
        self.sock.send(msg.encode())

    def receive(self) -> str:
        msg = str('')
        while (len(msg) < 100):
            chunk = self.sock.recv(100 - len(msg))
            if chunk.find(10) != -1:
                msg += str(chunk, "utf-8")
                return msg
        msg += str(chunk, "utf-8")
        return msg

class Client():
    def __init__(self) -> None:
        self.port = None
        self.name = None
        self.can_connect = None
        self.machine = "localhost"
        self.get_info(len(av))

    def get_info(self, avlen:int) -> None:
        for i in range(1, avlen, 2):
            if (av[i] == "-p"):
                try:
                    self.port = int(av[i + 1])
                    i += 1
                except ValueError:
                    stderr.write("Error: -p port == int, please check --help for more info\n")
                    xit(84)
            elif (av[i] == "-n"):
                try:
                    self.name = str(av[i + 1])
                except ValueError:
                    stderr.write("Error: -n name == str, please check --help for more info\n")
                i += 1
            elif (av[i] == "-h"):
                try:
                    self.machine = str(av[i + 1])
                except ValueError:
                    stderr.write("Error: -h machine == str, please check --help for more info\n")
            else:
                stderr.write("Error: Bad argument -> check --help for more info\n")
                xit(84)

def connection(cli:Client, sock:Socket, map_info:Map) -> None:
    sock.connect(cli.machine, cli.port)
    output = sock.receive()
    if (output == "WELCOME\n"):
        sock.send(cli.name + '\n')
        output = sock.receive()
        splitted = output.split('\n')
        try:
            cli.can_connect = int(splitted[0])
        except ValueError:
            stderr.write("ValueError: bad value given for CLIENT-NUM\n")
            xit(84)
        splitted = splitted[1].split(' ')
        try:
            map_info.msz[0] = int(splitted[0])
            map_info.msz[1] = int(splitted[1])
        except ValueError:
            stderr.write("ValueError: bad value given for TEAM-NAME\n")
            xit(84)
    elif (output == "ko\n"):
        stderr.write("ServerError: any welcome messages ... rude\n")
        sock.sock.close()
        xit(84)

def main():
    cli = Client()
    sock = Socket()
    map_info = Map()
    connection(cli, sock, map_info)




def helper():
    stdout.write("USAGE:\t./zappy_ai -p port -n name -h machine\n"
    "\tport\tis the port number\n\tname\tis the name of the team\n"
    "\tmachine\tis the name of the machine; localhost by default\n")
    xit(0)

def cmd_checkout() -> None:
    if (len(av) < 2 or len(av) > 7):
        stderr.write("Error: too many or not enough arguments, check --help\n")
        xit(84)
    if (av[1] == "--help"):
        helper()

if __name__ == '__main__':
    try:
        cmd_checkout()
        main()
    except KeyboardInterrupt:
        xit(0)