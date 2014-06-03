============================
OxyPlot Drawing Toolbar Demo
============================

Description
-----------

An example of using mouse events and annotations together to create a drawing toolbar with
`OxyPlot`_. Feel free to use any way you'd like! It was designed mostly because I wanted to add a
drawing feature to the chart display of an in-house application at work (and for fun too), mainly so
that the users could draw on the chart with added features like aligning text and drawing
above/below the series without having to export or print it first.

Please let me know if you find any issues, or even if there's a better way of doing something in the
code~! I'm always open to being told I'm doing it wrong, as it's just another reason to learn better
ways to improve how I code =). Also, I wrote most of the code to be self explanatory, but let me
know if it'd be better to add more comments / docstrings.

Also! All icons used on this toolbar are from the great Fugue Icons pack by Yusuke Kamiyamane! You
can find it here: `fugue icons`_

.. _OxyPlot: http://oxyplot.org/
.. _fugue icons: http://p.yusukekamiyamane.com/

Requirements
------------

This was created in VS2013 and .Net 4.5. You'll need to grab the OxyPlot.Core and OxyPlot.WinForms
packages yourself.

Screenshot!
-----------

Here's a screenshot of a bunch of annotations drawn onto the graph and the annotation selection tool
in use:

.. image:: ss-selectannot.png
    :width: 80pt
