"""
Routes and views for the flask application.
"""

from datetime import datetime
from flask import render_template, request
from WebMap import app
from WebMap.models.DBcase import Case


@app.route('/index')
def home():
    """Renders the home page."""
    return render_template(
        'index.html',
        title='Home Page',
        year=datetime.now().year,
    )


@app.route('/')
def map():
    Cases = Case.Query()
    if len(Cases) == 0:
        Cases = "[]"
    return render_template(
        'WebMap.html',
        title='Map Page',
        cases=Cases, 
    )

@app.route('/query')
def query():
    startdate = request.args["StartDate"]
    enddate = request.args["EndDate"]
    Cases = Case.Query(Startsdate=startdate, Enddate=enddate)
    if len(Cases)==0:
        Cases = ""
    return render_template(
        'WebMap.html',
        title='Map Page',
        cases=Cases, 
    )

@app.route('/contact')
def contact():
    """Renders the contact page."""
    return render_template(
        'contact.html',
        title='Contact',
        year=datetime.now().year,
        message='Your contact page.'
    )

@app.route('/about')
def about():
    """Renders the about page."""
    return render_template(
        'about.html',
        title='About',
        year=datetime.now().year,
        message='Your application description page.'
    )


