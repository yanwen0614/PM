import socket
import threading


class socketConnect(object):
    client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client.connect(("127.0.0.1", 9999))
    IsRecvPost = False

    def sendmsg(self, Post):
        while not self.IsRecvPost:
            self.client.send(str(Post).encode('utf-8'))
            print("sendpost")
        return

    def recvmsg(self):
        while True:
            msg = self.client.recv(1024).decode('utf-8')
            if msg == "GetPost":
                print("GetRecv")
                self.client.close()
                self.IsRecvPost = True
                return True

    def sendpost(self, Post):
        t1 = threading.Thread(target=self.sendmsg, args=([Post]))
        t1.start()
        print("sendmsgstart")
        t2 = threading.Thread(target=self.recvmsg,)
        t2.start()
        print("recvmsgstart")


if __name__ == '__main__':
    sc = socketConnect()
    sc.sendpost(55123)