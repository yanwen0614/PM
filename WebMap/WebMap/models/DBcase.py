import datetime
import json

import pymysql


class Case():
    tablename = "`case`"

    def __init__(self, id, Title, Content, 
                WebUrl, PublishTime, CollectTime,Casetype, Websource,
                Coorrdinates , Address, Checked,):
        self.id = id
        self.Title = Title
        self.Content = Content
        self.WebUrl = WebUrl
        self.PublishTime = str(PublishTime)
        self.CollectTime = str(CollectTime)
        self.Casetype = Casetype
        self.Websource = Websource
        self.Coorrdinates = Coorrdinates[6:-1].split(' ')
        self.Address = Address
        self.Checked = Checked

    @staticmethod
    def Query(**kwargs):
        return Database().query(**kwargs)

    def __repr__(self):
        return json.dumps(self)


class DATABASE(object):
        user = 'yanwen1'
        pw = 'duoduo'
        host = 'localhost'
        post = 3306  # default
        charset = "utf8mb4"
        dbname = "locationinfor"


class Database(object):
    _db = None
    cur = None
    where = "WHERE `case`.Casetype IN (0,1,2,3,4) AND `case`.CollectTime BETWEEN '{0}' AND '{1}' ORDER BY `case`.PublishTime DESC"
    select = "`case`.id,  `case`.Title, `case`.Content,  `case`.WebUrl, (`case`.PublishTime), (`case`.CollectTime), `case`.Casetype, `case`.Websource, AsText(`case`.Coorrdinates), `case`.Address, `case`.Checked"
    Datasql = "SELECT {select} FROM {table} {where}"
    
    def __init__(self):  
        self._db = pymysql.connect(host='localhost',
                                    user=DATABASE.user,
                                    password=DATABASE.pw,
                                    db=DATABASE.dbname,
                                    charset=DATABASE.charset,
                                    cursorclass=pymysql.cursors.DictCursor)
        self.cur = self._db.cursor()

    def query(self, casetype=None, Startsdate=None, Enddate=None, table="`case`",  where=None, ):
        if Startsdate == None:
            Startsdate = (str(datetime.date.today())+" 00:00:00")
        if Enddate == None:
            Enddate = (str(datetime.date.today())+" 23:59:59")
        if where == None:
            where = self.where
        sql = self.Datasql
        where = where.format(Startsdate, Enddate)
        sql = sql.format(select=self.select, where=where, table=table)
        self.cur.execute(sql)
        alldata = self.cur.fetchall()
        
        allcase = []
        for case in alldata:
            case['PublishTime'] = str(case['PublishTime'])
            case['CollectTime'] = str(case['CollectTime'])
            coor = case['AsText(`case`.Coorrdinates)'][6:-1].split(' ')
            case['Coorrdinates'] = [float(coor[1]),float(coor[0])]
            '''
            for field in case.items():
                arg.append(field[1])
            allcase.append(Case(*arg))
            '''
        
        return alldata #  [Case(*[field[1] for field in case]) for case in alldata]

    def close(self):
        if self._db.open:
            self._db.close()

def main():
    db = Database()
    a = db.query()
    print(a)
    
    

if __name__ == '__main__':
    main()
