from sys import argv as av
from sys import stderr
from sys import exit as xit

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