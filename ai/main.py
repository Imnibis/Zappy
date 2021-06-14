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

def main():
    pass

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