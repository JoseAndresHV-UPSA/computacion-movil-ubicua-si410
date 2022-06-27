import 'package:flutter/material.dart';

// ignore: must_be_immutable
class MultipleChoiceChipWidget extends StatefulWidget {
  final List<String>? options;
  List<String> selectedOptions;
  MultipleChoiceChipWidget(
      {Key? key, required this.options, required this.selectedOptions})
      : super(key: key);

  @override
  State<MultipleChoiceChipWidget> createState() =>
      _MultipleChoiceChipWidgetState();
}

class _MultipleChoiceChipWidgetState extends State<MultipleChoiceChipWidget> {
  _buildChoiceList() {
    List<Widget> choices = [];
    for (var option in widget.options!) {
      choices.add(Container(
        padding: const EdgeInsets.symmetric(vertical: 0.0, horizontal: 4.0),
        child: ChoiceChip(
          label: Text(option),
          labelStyle: const TextStyle(
              color: Colors.black, fontSize: 14.0, fontWeight: FontWeight.bold),
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(30.0)),
          backgroundColor: Colors.grey[200],
          selectedColor: Colors.amber[200],
          selected: widget.selectedOptions.contains(option),
          onSelected: (selected) {
            setState(() {
              if (widget.selectedOptions.contains(option)) {
                widget.selectedOptions.remove(option);
              } else {
                widget.selectedOptions.add(option);
              }
            });
          },
        ),
      ));
    }
    return choices;
  }

  @override
  Widget build(BuildContext context) {
    return Wrap(
      children: _buildChoiceList(),
    );
  }
}
