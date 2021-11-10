/// <reference types="Cypress" />
import {LabelDataTypeBuilder} from 'sbn-cypress-testhelpers';
context('Data Types', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  it('Create data type', () => {
    const name = "Test data type";

   cy.sbnEnsureDataTypeNameNotExists(name);

    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Data Types"]).rightclick();

    cy.sbnContextMenuAction("action-create").click();
    cy.sbnContextMenuAction("action-data-type").click();

    //Type name
    cy.sbnEditorHeaderName(name);


    cy.get('select[name="selectedEditor"]', {timeout: 5000}).select('Label');

    cy.get('.umb-property-editor select').select('Time');

    //Save
    cy.get('.btn-success').click();

    //Assert
    cy.sbnSuccessNotification().should('be.visible');

    //Clean up
    cy.sbnEnsureDataTypeNameNotExists(name);
   });

   it('Delete data type', () => {
    const name = "Test data type";
    cy.sbnEnsureDataTypeNameNotExists(name);

    const dataType = new LabelDataTypeBuilder()
      .withSaveNewAction()
      .withName(name)
      .build();

    cy.saveDataType(dataType);

    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Data Types", name]).rightclick();

    cy.sbnContextMenuAction("action-delete").click();

    cy.sbnButtonByLabelKey("general_delete").click();

    cy.contains(name).should('not.exist');

    cy.sbnEnsureDataTypeNameNotExists(name);


  });
});
