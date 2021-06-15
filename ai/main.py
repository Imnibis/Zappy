#!/usr/bin/env python3
from sys import argv as av
from sys import stderr, stdout
from sys import exit as xit

class Map():
    def __init__(self) -> None:
        msz = [0, 0]                # map size: X Y

class Player():
    def __init__(self) -> None:
        pn = 0
        ppo = [0,  0]               # player position: X Y O
        plv = 1                     # player level
        pin = [0, 0, 0, 0, 0, 0, 0] # player inventory
        tna =  list()               # name of all the teams

class Client():
    def __init__(self) -> None:
        self.port = None
        self.name = None
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

def main():
    client = Client()

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
    for i in av:
        pass

if __name__ == '__main__':
    cmd_checkout()
    main()